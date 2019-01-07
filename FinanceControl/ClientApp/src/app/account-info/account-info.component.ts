import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Currency } from "../model/currency.model";
import { Repository } from "../model/repository";
import { ActivatedRoute } from "@angular/router";
import { FormControl} from "@angular/forms";

@Component({
  selector: 'account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit{ 
  public accountsStatusInfo: AccountsStatus;
  public accounts: Account[];
  public currencies: Currency[]; // currencies which is consisting of filter (show only currency of active or hidden accounts)
  public activeMode: boolean; // show active accounts or hidden account
  public selectCurrency: FormControl; // link to dropbox elements with currencies list

  constructor(
    private repository: Repository,
    private router: ActivatedRoute
  ) { }

  ngOnInit(){
    this.accountsStatusInfo=new AccountsStatus();
    this.restoreActiveMode(); // Must be restored from session (temporary from routing)
    
    this.selectCurrency=new FormControl('ALL');
    this.loadAppropriateAccountsType(0);
    this.getCurrencies();
  }
  
  public restoreActiveMode(): void{
    // catch passed value about mode from route
    let paramVal: string = this.router.snapshot.paramMap.get('activeMode');
    
    if(paramVal=="true" || paramVal==null)
      this.activeMode=true;
    else
      this.activeMode=false;
  }

  // get appropriate currencies list
  public getCurrencies(): void{
    let method: string;
    if(this.activeMode==true)
      method="active";
    else
      method="hidden";

    this.repository.getAllCurrencies(method).subscribe(response =>{
      this.currencies=response;
    });
  }

  // filter accounts by currency
  public change_SelectCurrency(): void{
    if(this.selectCurrency.value=="ALL"){
      // load all account
      this.loadAppropriateAccountsType(0);
    } else {
      // load accounts by selected curency
      this.loadAppropriateAccountsType(this.selectCurrency.value);
    }
  }

  // Event handler. It invoked when change type of account via toggle
  public change_accountsType()
  {
    this.activeMode=!this.activeMode;
    this.selectCurrency.setValue('ALL');
    this.getCurrencies();
    this.loadAppropriateAccountsType(0);
    this.change_SelectCurrency();
  }

  // Working acorrding to value of activeMode variable
  private loadAppropriateAccountsType(currencyId: number){
    if(this.activeMode){
      this.repository.getActiveAccounts(currencyId).subscribe(response => {
        this.accounts=response;
        this.accountsStatusInfo=this.accountsStatus;
      });
    } else{
      this.repository.getHiddenAccounts(currencyId).subscribe(response => {
        this.accounts=response;
        this.accountsStatusInfo=this.accountsStatus;
      });
    }
  }

  // It forms total info string about define type of accounts
  get accountsStatus(): AccountsStatus{
    let typeMode: string;
    if(this.activeMode)
      typeMode="active";
    else
      typeMode="hidden";

    this.accountsStatusInfo.accountsNumbers = this.accounts.length;
    if(this.accountsStatusInfo.accountsNumbers==0)
      this.accountsStatusInfo.infoString="No accounts yet";
    else if(this.accountsStatusInfo.accountsNumbers==1)
      this.accountsStatusInfo.infoString=`1 ${typeMode}`;
    else
      this.accountsStatusInfo.infoString=`${this.accountsStatusInfo.accountsNumbers} ${typeMode}`;
    
    if(this.selectCurrency.value!="ALL")
      this.accountsStatusInfo.infoString+=" (currency filter was applied)";
    return this.accountsStatusInfo;
  }
}

class AccountsStatus{
  constructor(
    public accountsNumbers?: number,
    public infoString?: string
  ) { }
}
