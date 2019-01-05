import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Currency } from "../model/currency.model";
import { Repository } from "../model/repository";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit{ 
  public accountsStatusInfo: AccountsStatus;
  public accounts: Account[];
  public currencies: Currency[];
  public activeMode: boolean;
  
  constructor(
    private repository: Repository,
    private router: ActivatedRoute
  ) { }

  ngOnInit(){    
    this.accountsStatusInfo=new AccountsStatus();
    this.restoreActiveMode(); // Must be restored from session (temporary from routing)
    
    this.loadAppropriateAccountsType();
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

  public getCurrencies(): void{
    this.repository.allCurrencies.subscribe(response =>{
      this.currencies=response;
    });
  }

  public getActiveAccounts(): void{
    this.repository.getActiveAccounts().subscribe(response => 
      {
        this.accounts = response;
      });
  }

  // Event handler. It invoked when change type of account via toggle
  public change_accountsType()
  {
    this.activeMode=!this.activeMode;
    this.loadAppropriateAccountsType();
  }

  // Working acorrding to value of activeMode variable
  private loadAppropriateAccountsType(){
    if(this.activeMode){
      this.repository.getActiveAccounts().subscribe(response => {
        this.accounts=response;
        this.accountsStatusInfo=this.accountsStatus;
      });
    } else{
      this.repository.getHiddenAccounts().subscribe(response => {
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
    return this.accountsStatusInfo;
  }

}

class AccountsStatus{
  constructor(
    public accountsNumbers?: number,
    public infoString?: string
  ) { }
}
