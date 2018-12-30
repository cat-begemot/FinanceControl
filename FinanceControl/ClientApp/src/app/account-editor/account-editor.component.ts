import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Currency } from "../model/currency.model";
import { Repository } from "../model/repository";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.css']
})
export class AccountEditorComponent implements OnInit{
  public newAccount: Account;

  constructor(
    private repository: Repository,
    private router: ActivatedRoute
  ) { }

  ngOnInit()
  {    
    this.newAccount = new Account();
    this.newAccount.accountName="";
    this.repository.getCurrenciesList();
    this.loadAccountForEdit();
  }
  
  public loadAccountForEdit(): void
  {
    const id: number = +this.router.snapshot.paramMap.get('id');
    if(id!=0)
    {
      this.repository.getAccount(id);
      this.newAccount=this.repository.account;
    }
  }

  get currenciesList(): Currency[]{
    
    return this.repository.currenciesList;
  }

  clickCurrency(currency: Currency){
    this.newAccount.currency=currency;
  }

  clickCreateAccount(){
    // Arrange new account properties
    this.newAccount.accountId=0;
    this.newAccount.balance=this.newAccount.startAmount;
    this.newAccount.sequence=0;
    this.newAccount.activeAccount=true;
    this.newAccount.currency=null;

    // Create account
    this.repository.createAccount(this.newAccount);

   console.log(this.newAccount.accountName);
  }

  clearAccountFields(){
    // Clear
    this.newAccount.accountName="";
    this.newAccount.startAmount=0.00;
  }
}
