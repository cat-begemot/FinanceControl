import { Component, OnInit } from '@angular/core';
import { Location } from "@angular/common";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Transaction } from "../../model/transaction.model";
import { Repository } from "../../model/repository";
import { Account } from "../../model/account.model";
import { Item } from "../../model/item.model";
import { GroupType } from "../../model/group.model";
import { Comment } from 'src/app/model/comment.model';
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-transaction-edit',
  templateUrl: './transaction-edit.component.html',
  styleUrls: ['./transaction-edit.component.css']
})
export class TransactionEditComponent implements OnInit {
  public editorHeader: string;
  public editMode: boolean; // TRUE - edit transaction; FALSE - create new transaction
  public transactionForm: FormGroup;
  public currentTransaction: Transaction;
  public accounts: Account[]; // list of active accounts
  public items: Item[]; // list of items.  // TODO: get filtered list


  constructor(
    private location: Location,
    private repository: Repository,
    private router: ActivatedRoute
  ) { }

  ngOnInit() {
    this.currentTransaction=new Transaction();
    this.currentTransaction.dateTime=new Date();
    
    // Form initialize
    this.transactionForm=new FormGroup({
      typeControl: new FormControl(GroupType.Expense, Validators.required),
      dateControl: new FormControl(this.getCurrentDateTime(this.currentTransaction.dateTime), Validators.required),
      accountControl: new FormControl('', Validators.required),
      itemControl: new FormControl('', Validators.required),
      commentControl: new FormControl(''),
      amountControl: new FormControl('', Validators.required),
      currencyNameControl: new FormControl({value: '', disabled: true}),
      rateControl: new FormControl('', Validators.required),
      rateNameControl: new FormControl({value: '', disabled: true})
    });
    
    // load accounts // TODO: create app-status pull of all array instances?
    this.repository.getActiveAccounts(0).subscribe(response=>{
      this.accounts=response;
    });

    // load items
    this.repository.getItems(this.typeControl.value as GroupType).subscribe(response => {
      this.items=response;
    });
    
    this.check_editMode();   
  }

  // check whether is create new or edit exist transaction mode
  public check_editMode(): void{
    let id=+this.router.snapshot.paramMap.get("id");
    if(id==0){ // Create new transaction
      this.editMode=false;
      this.editorHeader="New transaction";
    } else { // Edit exists transaction
      this.editMode=true;
      this.editorHeader="Edit transaction";
      // get transaction from database and fill form
      this.repository.getTransactionById(id).subscribe(transaction=>{
        this.currentTransaction=transaction;
        let gt: GroupType=transaction.item.group.type;  
        if(gt==GroupType.Expense)
          this.typeControl.setValue(gt);
        else if(gt==GroupType.Income)
          this.typeControl.setValue(gt);
        else(gt==GroupType.Account)
          this.typeControl.setValue(gt);
        
        this.typeControl.disable();
        this.change_type();

        this.dateControl.setValue(transaction.dateTime);
        this.accountControl.setValue(transaction.accountId);
        this.itemControl.setValue(transaction.itemId);
        this.amountControl.setValue(transaction.currencyAmount);
        this.rateControl.setValue(transaction.rateToAccCurr);

        if(transaction.comment!=null && transaction.comment!=undefined)
          this.commentControl.setValue(transaction.comment.commentText);
        
        this.currencyNameControl.setValue(this.currentTransaction.account.currency.code);
        if(gt!=GroupType.Account){
          this.rateNameControl.setValue(this.currencyNameControl.value + "/UAH");
        } else{
          let itemId: number = this.accounts.find(account=>account.itemId==this.itemControl.value).itemId;
          this.rateNameControl.setValue(this.currencyNameControl.value + "/" + this.accounts.find(account=>account.itemId==itemId).currency.code);
        }
      });
    }
  }

  // get DateTime string for setting input DateTime form control
  public getCurrentDateTime(dtValue: Date): string{
    let dt: Date = dtValue;
    let arrDateTime: string[] = dt.toISOString().split("T");
    let hours: string = dt.getHours().toString();
    let minutes: string = dt.getMinutes().toString();
    let seconds: string = dt.getSeconds().toString();
    
    return arrDateTime[0] + "T" + 
      (hours.length==1 ? "0" + hours : hours) + ":" + 
      (minutes.length==1 ? "0" + minutes : minutes) + ":" +
      (seconds.length==1 ? "0" + seconds : seconds);
  }

  public click_Cancel(): void{
    this.location.back();
  }

  public onSubmit(): void{
    if(this.editMode==false){ // create new transaction
      this.currentTransaction.transactionId=0;
      this.currentTransaction.dateTime=this.dateControl.value;
      this.currentTransaction.accountId=this.accountControl.value;
      this.currentTransaction.itemId=this.itemControl.value;
      this.currentTransaction.currencyAmount=this.amountControl.value;
      this.currentTransaction.rateToAccCurr=this.rateControl.value;
      
      if(this.commentControl.value!=""){ // if user enter a comment
        this.currentTransaction.comment=new Comment();
        this.currentTransaction.comment.commentText=this.commentControl.value;
      }

      this.repository.createTransaction(this.currentTransaction).subscribe(()=>{
        this.location.back();
      });
    }
  }

  // filter items when type select was changed
  public change_type(): void{
    this.repository.getItems(this.typeControl.value as GroupType).subscribe(response => {
      this.items=response;
    });
  }

  // change event for Account select
  public change_account(): void{
    this.setRateControls();
  }

  // change event for Item select
  public change_item(): void{
    this.setRateControls();
  }

  // Set rate and disabled rate names fields according to elected type, account and item
  private setRateControls(): void{
    this.currencyNameControl.setValue(this.accounts.find(account=>account.accountId==this.accountControl.value).currency.code);
    if(this.typeControl.value==GroupType.Account){ // Set rate for movement
      if(this.itemControl.dirty){
        let itemId: number = this.accounts.find(account=>account.itemId==this.itemControl.value).itemId;
        this.rateNameControl.setValue(this.currencyNameControl.value + "/" + this.accounts.find(account=>account.itemId==itemId).currency.code);
      }
    } else { // set rate for Income and Expense
      this.rateNameControl.setValue(this.currencyNameControl.value + "/UAH");
    }
    // if Account and Item currency the same, set rate as 1.0000
    if(this.rateNameControl.value!=""){
      let accountCurrencyCode: string = (this.rateNameControl.value as string).slice(0,3);
      let itemCurrencyCode: string = (this.rateNameControl.value as string).slice(4,7);
      if(accountCurrencyCode==itemCurrencyCode){
        this.rateControl.setValue(1);
      } else {
        this.rateControl.setValue("");
      }
    }
  }

  // click event for Inverse button
  public click_inverse(): void{
    if(this.rateControl.value!="")
    {
      this.rateControl.setValue(1/this.rateControl.value);
    }
  }

  // Get form controls
  public get dateControl(): FormControl{
    return this.transactionForm.get("dateControl") as FormControl;
  }

  public get accountControl(): FormControl{
    return this.transactionForm.get("accountControl") as FormControl;
  }

  public get typeControl(): FormControl{
    return this.transactionForm.get("typeControl") as FormControl;
  }

  public get currencyNameControl(): FormControl{
    return this.transactionForm.get("currencyNameControl") as FormControl;
  }

  public get itemControl(): FormControl{
    return this.transactionForm.get("itemControl") as FormControl;
  }

  public get amountControl(): FormControl{
    return this.transactionForm.get("amountControl") as FormControl;
  }

  public get rateControl(): FormControl{
    return this.transactionForm.get("rateControl") as FormControl;
  }

  public get commentControl(): FormControl{
    return this.transactionForm.get("commentControl") as FormControl;
  }

  public get rateNameControl(): FormControl{
    return this.transactionForm.get("rateNameControl") as FormControl;
  }
}
