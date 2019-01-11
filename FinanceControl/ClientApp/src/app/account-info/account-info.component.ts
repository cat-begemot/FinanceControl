import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Currency } from "../model/currency.model";
import { Repository } from "../model/repository";
import { ActivatedRoute } from "@angular/router";
import { FormControl} from "@angular/forms";
import { AppStatusService } from "../app-status.service";

@Component({
  selector: 'account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit{ 
  public accountsStatusInfo: AccountsStatus;
  public accounts: Account[];
  public currencies: Currency[]; // currencies which is consisting of filter (show only currency of active or hidden accounts)
  public selectCurrency: FormControl; // link to dropbox elements with currencies list

  constructor(
    private repository: Repository,
    private router: ActivatedRoute,
    public appStatus: AppStatusService
  ) { }

  ngOnInit(){
    this.accountsStatusInfo=new AccountsStatus();
    this.selectCurrency=new FormControl('ALL');
    this.loadAppropriateAccountsType(0);
    this.getCurrencies();
  }

  // get appropriate currencies list
  public getCurrencies(): void{
    let method: string;
    if(this.appStatus.activeAccountsMode==true)
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
    this.appStatus.activeAccountsMode=!this.appStatus.activeAccountsMode;
    this.selectCurrency.setValue('ALL');
    this.getCurrencies();
    this.loadAppropriateAccountsType(0);
    this.change_SelectCurrency();
  }

  // Working acorrding to value of appStatus.activeAccountsMode
  private loadAppropriateAccountsType(currencyId: number){
    if(this.appStatus.activeAccountsMode){
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
    if(this.appStatus.activeAccountsMode)
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
