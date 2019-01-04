import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from "@angular/forms";
import { Repository } from 'src/app/model/repository';
import { Currency } from "../../../model/currency.model";
import { Router } from "@angular/router";
import { Location } from "@angular/common";

@Component({
  selector: 'app-currency-editor',
  templateUrl: './currency-editor.component.html',
  styleUrls: ['./currency-editor.component.css']
})
export class CurrencyEditorComponent implements OnInit {
  // Form model
  public currencyForm: FormGroup = new FormGroup({
    code: new FormControl(''),
    description: new FormControl('')
  });

  public currentCurrency: Currency;

  constructor(
    private repository: Repository,
    private router: Router,
    private location: Location
  ) { }

  ngOnInit() {
    this.currentCurrency=new Currency();
  }

  public onSubmit(){
    this.currentCurrency.currencyId=0;
    this.currentCurrency.code=this.currencyForm.get("code").value;
    this.currentCurrency.description=this.currencyForm.get("description").value;
    this.currentCurrency.accounts=null;
    
    this.repository.createCurrency(this.currentCurrency).subscribe(()=>{
      this.location.back();
    });
  }

  public click_Cancel(){
    this.location.back();
  }
}
