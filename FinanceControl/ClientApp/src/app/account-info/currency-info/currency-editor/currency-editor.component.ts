import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { Repository } from 'src/app/model/repository';
import { Currency } from "../../../model/currency.model";
import { Router, ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

@Component({
  selector: 'app-currency-editor',
  templateUrl: './currency-editor.component.html',
  styleUrls: ['./currency-editor.component.css']
})
export class CurrencyEditorComponent implements OnInit {
  public currentCurrency: Currency;
  public editMode: boolean; // Create new or edit exist curreny
  public editorHeader: string; // Header value on the top of component
  public currencyForm: FormGroup;  // Form model
  public isCurrencyCodeExist: boolean; // If it is true - new currency code is already existed

  constructor(
    private repository: Repository,
    private router: ActivatedRoute,
    private routerNav: Router,
    private location: Location
  ) { }

  ngOnInit() {
    this.currentCurrency=new Currency();
    this.setEditMode();
    this.currencyForm = new FormGroup({
      code: new FormControl(this.currentCurrency.code, Validators.required),
      description: new FormControl(this.currentCurrency.description)
    });
    this.isCurrencyCodeExist=false;
  }

  private setEditMode(){
    let id: number = +this.router.snapshot.paramMap.get('id');
    if(id!=0)
    {
      this.editMode=true;
      this.editorHeader="Edit currency";
      // in edit mode load currency info in currentCurrency variable
      this.repository.getCurrencyById(id).subscribe(response => {
        this.currentCurrency=response;
        this.currencyForm.setValue({code: this.currentCurrency.code, description: this.currentCurrency.description});
      });
    }
    else{
      this.editMode=false; // create a new currency
      this.editorHeader="Create currency";
    }
  }

  public onSubmit(){
    // first check if currency code is unique
    this.repository.isCurrencyCodeExist(this.code().value).subscribe(response=>{
      if(response==true){
        this.isCurrencyCodeExist=true;
      } else{
        this.isCurrencyCodeExist=false;
        // get data from form
        this.currentCurrency.code=this.currencyForm.get("code").value;
        this.currentCurrency.description=this.currencyForm.get("description").value;
        this.currentCurrency.accounts=null;

        if(this.editMode){ // edit currency
          this.repository.updateCurrency(this.currentCurrency).subscribe(()=>{
            this.location.back();
          });
        } else { // create new currency
          this.currentCurrency.currencyId=0;
          
          this.repository.createCurrency(this.currentCurrency).subscribe(()=>{
            this.location.back();
          });
        }
      }
    });
  }

  public click_Cancel(){
    this.location.back();
  }

  public click_Delete(){
    this.repository.deleteCurrency(this.currentCurrency.currencyId).subscribe(()=>{
      this.routerNav.navigate(["/currencies"]);
    });
  }

  // check if control is invalid, dirty and and touched
  public isInvalid(control: AbstractControl): boolean{
    return control.invalid && ( control.dirty || control.touched);
  }

  // returns code control
  public code(){
    return this.currencyForm.get('code');
  }

  public change_Code(): void{
    this.repository.isCurrencyCodeExist(this.code().value).subscribe(response=>{
      if(response==true){
        this.isCurrencyCodeExist=true;
      } else{
        this.isCurrencyCodeExist=false;
      }
    });
  }
}
