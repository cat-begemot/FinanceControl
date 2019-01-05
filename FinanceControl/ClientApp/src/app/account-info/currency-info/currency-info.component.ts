import { Component, OnInit } from '@angular/core';
import { Currency } from "../../model/currency.model";
import { Repository } from "../../model/repository";

@Component({
  selector: 'app-currency-info',
  templateUrl: './currency-info.component.html',
  styleUrls: ['./currency-info.component.css']
})
export class CurrencyInfoComponent implements OnInit {
  public currencies: Currency[];

  constructor(
    private repository: Repository
  ) { }

  ngOnInit() {
    this.repository.allCurrencies.subscribe(response => {
      this.currencies = response;
    });
  }

}
