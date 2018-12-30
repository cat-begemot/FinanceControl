import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent implements OnInit{ 
  private _accountsStatus: AccountsStatus;
  
  constructor(
    private repository: Repository
  ) { }

  ngOnInit(){
    this._accountsStatus=new AccountsStatus(0, "Loading...");
    this.repository.getActiveAccounts();
  }

  clickAccount(accountId: number){
      
  }

  // Properties getters
  get activeAccounts(): Account[]{
    return this.repository.activeAccounts;
  }

  get accountsStatus(): AccountsStatus{    
    if(this.repository.activeAccounts!==null && this.repository.activeAccounts!==undefined){
      this._accountsStatus.accountsNumbers = this.repository.activeAccounts.length;
      if(this._accountsStatus.accountsNumbers==0)
        this._accountsStatus.infoString="No accounts yet";
      else if(this._accountsStatus.accountsNumbers==1)
        this._accountsStatus.infoString="1 account";
      else
        this._accountsStatus.infoString=`${this._accountsStatus.accountsNumbers} accounts`;
      return this._accountsStatus;
    }
  }
}

class AccountsStatus{
  constructor(
    public accountsNumbers?: number,
    public infoString?: string
  ) { }
}
