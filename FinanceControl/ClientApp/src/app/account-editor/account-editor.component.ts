import { Component, OnInit } from '@angular/core';
import { Account } from "../model/account.model";
import { Currency } from "../model/currency.model";
import { Repository } from "../model/repository";
import { ActivatedRoute } from "@angular/router";
import { Router } from "@angular/router";
import { FormControl } from "@angular/forms";

@Component({
  selector: 'account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.css']
})
export class AccountEditorComponent implements OnInit{
  public currentAccount: Account;
  public currencies: Currency[];
  public editMode: boolean;
  public editorHeader: string;
  public showActiveAccountsMode: boolean;
  public currencySelect: FormControl = new FormControl('');

  constructor(
    private repository: Repository,
    private router: ActivatedRoute,
    private routerNav: Router
  ) { }

  ngOnInit()
  {    
    this.editMode=false;
    this.showActiveAccountsMode=true;
    this.editorHeader="Create account";
    this.currentAccount=new Account();
    this.currentAccount.currencyId=1;
    this.currentAccount.activeAccount=this.showActiveAccountsMode;
    this.checkEditorMode();
    this.getAllCurrencies();

    this.currencySelect.valueChanges.subscribe((response: number) => this.currentAccount.currencyId=response);
  }

  public checkEditorMode(): void
  {
    const id: number = +this.router.snapshot.paramMap.get('id');
    if(id!=0) // if id==0 - CREATE a new account, else - EDIT existed account
    {
      this.editMode=true;
      this.editorHeader="Edit account";
      this.repository.getAccountById(id).subscribe(response => {
        this.currentAccount=response;
        this.currencySelect.setValue(this.currentAccount.currency.currencyId);
      });
    }
  }

  getAllCurrencies(): void{
    this.repository.allCurrencies.subscribe(response => this.currencies = response);
  }

  // Create a new or change existed account
  clickCreateAccount(){
    // Arrange new account properties
    this.currentAccount.accountId=0;
    this.currentAccount.balance=this.currentAccount.startAmount;
    this.currentAccount.sequence=0;
    this.currentAccount.activeAccount=true;
    this.currentAccount.currency=null;

    // Create account
    this.repository.createAccount(this.currentAccount).subscribe(()=>{
      // Navigate to parent route after receive response from Web server
      this.routerNav.navigate(["/accounts"]);
    });
  }

  // Edit existed account
  clickEditAccount(){
    this.currentAccount.currency=null;
    this.repository.updateAccount(this.currentAccount).subscribe(()=>{
      this.routerNav.navigate(["/accounts"]);
    });
  }

  // Delete selected account
  clickDeleteAccount(){
    this.repository.deleteAccount(this.currentAccount.accountId).subscribe(()=>{
      this.routerNav.navigate(["/accounts"]);
    });    
  }

  public click_HideActivateAccount(mode: boolean): void{
    this.currentAccount.activeAccount=mode;
    this.clickEditAccount();
  }
}
