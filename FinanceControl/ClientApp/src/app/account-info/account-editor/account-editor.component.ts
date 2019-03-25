import { Component, OnInit } from '@angular/core';
import { Account } from "../../model/account.model";
import { Currency } from "../../model/currency.model";
import { Repository } from "../../model/repository";
import { ActivatedRoute } from "@angular/router";
import { Router } from "@angular/router";
import { FormControl, Validators } from "@angular/forms";
import { AppStatusService } from "../../app-status.service";
import { Group, GroupType } from "../../model/group.model";

@Component({
  selector: 'account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.css']
})
export class AccountEditorComponent implements OnInit{
  public currencies: Currency[];
  public groups: Group[];
  public editMode: boolean;
  public editorHeader: string;
  public currencySelect: FormControl = new FormControl('', Validators.required);
  public groupSelect: FormControl = new FormControl('', Validators.required);

  constructor(
    private repository: Repository,
    private router: ActivatedRoute,
    private routerNav: Router,
    public appStatus: AppStatusService
  ) { }

  ngOnInit()
  {    
    this.appStatus.currentAccount.activeAccount=this.appStatus.activeAccountsMode;
    
    this.checkEditorMode();

    this.getAllCurrencies();

    // Subscribe on currency selector
    this.currencySelect.valueChanges.subscribe((response: number) => {
      this.appStatus.currentAccount.currencyId=response;
    });
    // Subscrive on group selector
    this.groupSelect.valueChanges.subscribe((response: number)=>{
      this.appStatus.currentAccount.item.groupId=response;
    });

    this.repository.getAllGroup(GroupType.Account).subscribe(response=>{
      this.groups=response;
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
        if(this.appStatus.currentAccount.accountId!=response.accountId){
          this.appStatus.currentAccount=response;
        }
        this.currencySelect.setValue(this.appStatus.currentAccount.currencyId);
        this.groupSelect.setValue(this.appStatus.currentAccount.item.groupId);
      });
    } else {
      this.editMode=false;
      this.editorHeader="Create account";
      if(this.appStatus.currentAccount.currencyId!=0){
        this.currencySelect.setValue(this.appStatus.currentAccount.currencyId);
      }
    }
  }

  getAllCurrencies(): void{
    this.repository.allCurrencies.subscribe(response => this.currencies = response);
  }

  // Create a new or change existed account
  clickCreateAccount(){
    // Arrange new account properties
    this.appStatus.currentAccount.accountId=0;
    this.appStatus.currentAccount.balance=this.appStatus.currentAccount.startAmount;
    this.appStatus.currentAccount.sequence=0;
    this.appStatus.currentAccount.activeAccount=true;
    this.appStatus.currentAccount.currency=null;
    this.appStatus.currentAccount.item.groupId=this.groupSelect.value;

    // Create account
    this.repository.createAccount(this.appStatus.currentAccount).subscribe(()=>{
      // Navigate to parent route after receive response from Web server
      this.routerNav.navigate(["/accounts"]);
      this.appStatus.clearAccount();
    });
  }

  // Edit existed account
  clickEditAccount(){
    this.appStatus.currentAccount.currency=null;
    this.repository.updateAccount(this.appStatus.currentAccount).subscribe(()=>{
      this.routerNav.navigate(["/accounts"]);
      this.appStatus.clearAccount();
    });
  }

  // Delete selected account
  clickDeleteAccount(){
    this.repository.deleteAccount(this.appStatus.currentAccount.accountId).subscribe(()=>{
      this.routerNav.navigate(["/accounts"]);
      this.appStatus.clearAccount();
    });
  }

  public click_HideActivateAccount(mode: boolean): void{
    this.appStatus.currentAccount.activeAccount=mode;
    this.clickEditAccount();
  }

  // control validation
  public isInvalid(control: any): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  // add currency during editing account
  public click_AddCurrency(){
    this.routerNav.navigate(["/currencies/add"]);
  }

  // Event for "Cancel" button
  public click_Cancel(){
    this.routerNav.navigate(["/accounts"]);
    this.appStatus.clearAccount();
  }

  public click_AddGroup(): void{
    this.appStatus.groupEditorCaller=GroupType.Account;
    this.routerNav.navigate(["/groups/add"]);
  }

  public click_groupEdit(){
    this.appStatus.groupEditorCaller=GroupType.Account;
    this.routerNav.navigate(["/groups/edit/", this.groupSelect.value]);
  } 
}
