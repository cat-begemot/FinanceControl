import { Component, OnInit } from '@angular/core';
import { Currency } from "../../model/currency.model";
import { Repository } from "../../model/repository";
import { Target } from "../../model/helper.model";

@Component({
  selector: 'app-currency-info',
  templateUrl: './currency-info.component.html',
  styleUrls: ['./currency-info.component.css']
})
export class CurrencyInfoComponent implements OnInit {
  public currencies: Currency[];
  public targetComponent: Target;

  constructor(
    private repository: Repository
  ) { }

  ngOnInit() {
    this.targetComponent=Target.Currencies;
    this.repository.allCurrencies.subscribe(response => {
      this.currencies = response;
    });
  }

}
