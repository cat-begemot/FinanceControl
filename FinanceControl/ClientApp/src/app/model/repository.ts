import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Currency } from "./currency.model";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

const accountUrl: string = "api/accounts";
const currencyUrl: string = "api/currencies";

@Injectable({
  providedIn: 'root'
})
export class Repository {
  constructor(
    private http: HttpClient
  ) { }

  // ACCOUNT SECTION ===========================================================================================

  // Get array with active accounts
  public activeAccounts(): Observable<Account[]>{
    return this.http.get<Account[]>(accountUrl + "/active");
  }

  //Get account by Id
  public getAccountById(accountId: number): Observable<Account>{
    return this.http.get<Account>(accountUrl + "/" + accountId.toString());
  }

  // Create new account
  public createAccount(newAccount: Account): Observable<any>{
    return this.http.post(accountUrl, newAccount);
  }

  // Delete account
  public deleteAccount(accountId: number): Observable<any>{
    return this.http.delete(accountUrl + "/" + accountId);
  }

  // Change account
  public updateAccount(updatedAccount: Account): Observable<any>{
    return this.http.put(accountUrl, updatedAccount);
  }

  // CURRENCY SECTION ===========================================================================================
  // Get all currencies
  public get allCurrencies(): Observable<Currency[]>{
    return this.http.get<Currency[]>(accountUrl + "/currencies");
  }

  // Get currency by ID
  public getCurrencyById(currencyId: number): Observable<Currency>{
    return this.http.get<Currency>(currencyUrl + "/" + currencyId);
  }

  // Create new curency
  public createCurrency(newCurrency: Currency): Observable<any>{
    return this.http.post(currencyUrl, newCurrency);
  }

  // Change currency
  public updateCurrency(updatedCurrency: Currency): Observable<any>{
    return this.http.put(currencyUrl, updatedCurrency);
  }

  // Delete currency
  public deleteCurrency(currencyId: number): Observable<any>{
    return this.http.delete(currencyId + "/" + currencyId);
  }
}