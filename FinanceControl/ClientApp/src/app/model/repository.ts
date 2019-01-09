import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Currency } from "./currency.model";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

const accountUrl: string = "api/accounts";
const currencyUrl: string = "api/currencies";
const sessionUrl: string = "api/sessions";
const loginUrl: string = "api/account";

@Injectable({
  providedIn: 'root'
})
export class Repository {
  constructor(
    private http: HttpClient
  ) { }

  // ACCOUNT SECTION ===========================================================================================

  // Get array with active accounts
  public getActiveAccounts(currencyId: number): Observable<Account[]>{
    return this.http.get<Account[]>(accountUrl + "/active/" + currencyId);
  }

  // Get array witn hidden accounts
  public getHiddenAccounts(currencyId: number): Observable<Account[]>{
    return this.http.get<Account[]>(accountUrl + "/" + "inactive/" + currencyId);
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
    return this.http.get<Currency[]>(accountUrl + "/currencies/none");
  }

  public getAllCurrencies(method: string): Observable<Currency[]>{
    return this.http.get<Currency[]>(accountUrl + `/currencies/${method}`);
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
    return this.http.delete(currencyUrl + "/" + currencyId);
  }

  // SESSION SECTION ===========================================================================================
  // get sessiod data by key
  public getSessionData(key: string): Observable<any>{
    return this.http.get<any>(sessionUrl + "/" + key);
  }

  // save session data by key
  public setSessionData(key: string, value: any): Observable<any>{
    return this.http.post<any>(sessionUrl + "/" + key, value);
  }

  // AUTHENTICATION SECTION ===========================================================================================
  // login
  public login(name: string, password: string): Observable<any>{
    return this.http.post<any>(loginUrl + "/login", {name: name, password: password});
  }

  public logout(): Observable<any>{
    return this.http.get<any>(loginUrl + "/logout");
  }

  public getUser(): Observable<string>{
    return this.http.get(sessionUrl + "/currentUserId", {responseType: 'text'});
  }
}