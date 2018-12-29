import { Component } from '@angular/core';
import { Account } from "../model/account.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.css']
})
export class AccountEditorComponent {
  public newAccount: Account = new Account();

  constructor(
    private repository: Repository
  ) { }

  clickCreateAccount(){
    // Arrange new account properties
    this.newAccount.accountId=0;
    this.newAccount.balance=this.newAccount.startAmount;
    this.newAccount.sequence=0;
    this.newAccount.activeAccount=true;

    // Create account
    this.repository.createAccount(this.newAccount);
  }

  clearAccountFields(){
    // Clear
    this.newAccount.accountName="";
    this.newAccount.startAmount=0.00;
  }
}
