import { Component } from '@angular/core';
import { Account } from "../model/account.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.css']
})
export class AccountInfoComponent { 
  private _activeAccountTotal: string;
  private _inactiveAccountTotal: string;

  constructor(
    private repository: Repository
  ) {

  }


  // Event methods
  createNewAccount(){

  }

  // Properties getters
  get activeAccounts(): Account[]{
    return this.repository.activeAccount;
  }

  get inactiveAccounts(): Account[]{
    return this.repository.inactiveAccount;
  }

  get activeAccountTotal(): string{    
    if(this.repository.activeAccount!==null && this.repository.activeAccount!==undefined){
      let accountQty: number = this.repository.activeAccount.length;
      if(accountQty==0)
        this._activeAccountTotal="There are any account yet";
      else if(accountQty==1)
        this._activeAccountTotal="1 active account";
      else
        this._activeAccountTotal=`${accountQty} active accounts`;
      return this._activeAccountTotal;
    }
  }

  get inactiveAccountTotal(): string{
    if(this.repository.inactiveAccount!=null && this.repository.inactiveAccount!==undefined){
      let accountQty: number = this.repository.inactiveAccount.length;
      if(accountQty==0)
        this._inactiveAccountTotal="There are any account yet";
      else if(accountQty==1)
        this._inactiveAccountTotal="1 hidden account";
      else
        this._inactiveAccountTotal=`${accountQty} hidden accounts`;
      return this._inactiveAccountTotal;
    }
  }
}
