import { Component, OnInit } from '@angular/core';
import { Account } from "../../model/account.model";
import { Currency } from "../../model/currency.model";
import { Repository } from "../../model/repository";
import { ActivatedRoute } from "@angular/router";
import { Router } from "@angular/router";
import { FormControl, Validators } from "@angular/forms";


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
  public currencySelect: FormControl = new FormControl('', Validators.required);
  public activeMode: boolean; // Active or hidden account was passed to editor

  constructor(
    private repository: Repository,
    private router: ActivatedRoute,
    private routerNav: Router
  ) { }

  ngOnInit()
  {    
    this.currentAccount=new Account();

    // catch passed value about mode from route
    let paramVal: string = this.router.snapshot.paramMap.get('activeMode');
    if(paramVal=="true")
      this.activeMode=true;
    else
      this.activeMode=false;

    this.currentAccount.activeAccount=this.activeMode;
    
    this.checkEditorMode();

    this.getAllCurrencies();

    // Subscribe on currency selector
    this.currencySelect.valueChanges.subscribe((response: number) => {
      this.currentAccount.currencyId=response;
    });
  }

  // Create new or edit exist account. Restore account from session
  public checkEditorMode(): void
  {
    const id: number = +this.router.snapshot.paramMap.get('id');
    if(id!=0){ // if id!=0 - edit existed account, else - create new one
      this.editMode=true;
      this.editorHeader="Edit account";

      this.repository.getAccountById(id).subscribe(response => { // get account with Id from routing
        this.repository.getSessionData("currentAccount").subscribe((sessionResponse: Account)=>{ // get account from session
          if(sessionResponse.accountId!=response.accountId){ // compare session and routing accounts
            // if not the same Account, show new account
            this.currentAccount=response;
            this.currencySelect.setValue(this.currentAccount.currencyId);
            this.repository.setSessionData("currentAccount", this.currentAccount).subscribe(()=> { });
          } else{
            // if the same Account, show account from session
            this.currentAccount=sessionResponse;
            this.currencySelect.setValue(this.currentAccount.currencyId);
          }
        });
      });
    } else {
      this.editMode=false;
      this.editorHeader="Create account";
      this.repository.getSessionData("currentAccount").subscribe((response: Account)=>{
        this.currentAccount=response;
        if(this.currentAccount.currencyId!=0){
          this.currencySelect.setValue(this.currentAccount.currencyId);
        }
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
      this.routerNav.navigate(["/accounts", {activeMode: this.activeMode}]);
      this.clearSessionForCurrentAccount();
    });
  }

  // Delete selected account
  clickDeleteAccount(){
    this.repository.deleteAccount(this.currentAccount.accountId).subscribe(()=>{
      this.routerNav.navigate(["/accounts", {activeMode: this.activeMode}]);
      this.clearSessionForCurrentAccount();
    });
  }

  public click_HideActivateAccount(mode: boolean): void{
    this.currentAccount.activeAccount=mode;
    this.clickEditAccount();
  }

  // control validation
  public isInvalid(control: any): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  // add currency during editing account
  public click_AddCurrency(){
    this.repository.setSessionData("currentAccount", this.currentAccount).subscribe(()=> { });
    this.routerNav.navigate(["/currencies/add"]);
  }

  // clear currentAccount in session if transaction with Account was succesful
  private clearSessionForCurrentAccount(): void{
    this.repository.setSessionData("currentAccount", new Account()).subscribe(()=>{ });
  }

  // Event for "Cancel" button
  public click_Cancel(){
    this.routerNav.navigate(["/accounts", {activeMode: this.activeMode}]);
    this.clearSessionForCurrentAccount();
  }
}
