import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { Currency } from "./currency.model";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { Group, GroupType } from './group.model';
import { Item } from "./item.model";
import { Transaction } from './transaction.model';

const accountUrl: string = "api/accounts";
const currencyUrl: string = "api/currencies";
const loginUrl: string = "api/account";
const groupsUrl: string = "api/groups";
const itemsUrl: string = "api/items";
const transactionsUrl: string = "api/transactions";

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

  // Check wether currency code is exist. If true - it is already existed
  public isCurrencyCodeExist(code: string): Observable<boolean>{
    return this.http.get<boolean>(currencyUrl + "/isCurrencyCodeExist/" + code);
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

  // AUTHENTICATION SECTION ===========================================================================================
  // login
  public login(name: string, password: string): Observable<any>{
    return this.http.post<any>(loginUrl + "/login", {name: name, password: password});
  }

  public logout(): Observable<any>{
    return this.http.get<any>(loginUrl + "/logout");
  }

  public isAuthenticated(): Observable<boolean>{
    return this.http.get<boolean>(loginUrl + "/isAuth");
  }

  public isLoginExist(name: string): Observable<any>{
    return this.http.post(loginUrl + "/isNameExist" + "/" + name, {responseType: 'text'});
  }

  public createUserProfile(name: string, password: string): Observable<boolean>{
    return this.http.post<boolean>(loginUrl + "/createUserProfile", {name: name, password: password});
  }

  // This method must be applied only after logining the user
  public seedData(): Observable<any>{
    return this.http.get(loginUrl + "/seedData");
  }

  // GROUP SECTION ===========================================================================================
  public createGroup(newGroup: Group): Observable<any>{
    return this.http.post(groupsUrl, newGroup);
  }

  public isGroupNameExists(name: string): Observable<boolean>{
    return this.http.get<boolean>(groupsUrl + `/isGroupNameExists/${name}`);
  }

  public getAllGroup(type: GroupType): Observable<Group[]>{
    return this.http.get<Group[]>(groupsUrl + "/all/" + type);
  }

  public getGroupById(id: number): Observable<Group>{
    return this.http.get<Group>(groupsUrl + `/${id}`);
  }

  public updateGroup(updatedGroup: Group): Observable<any>{
    return this.http.put<any>(groupsUrl, updatedGroup);
  }

  public deleteGroup(id: number): Observable<any>{
    return this.http.delete<any>(`${groupsUrl}/${id}`);
  }


  // ITEM SECTION ===========================================================================================
  public getItems(type: GroupType): Observable<Item[]>{
    return this.http.get<Item[]>(itemsUrl + `/all/${type}`);
  }

  public getIncomeExpenseItems(): Observable<Item[]>{
    return this.http.get<Item[]>(itemsUrl + "/getIncomeExpense");
  }

  public isItemNameExists(item: Item): Observable<boolean>{
    return this.http.post<boolean>(itemsUrl + "/isNameExists", item);
  }

  public createItem(newItem: Item): Observable<any>{
    return this.http.post<any>(itemsUrl, newItem);
  }

  public getItemById(id: number): Observable<Item>{
    return this.http.get<Item>(itemsUrl + `/${id}`);
  }

  public deleteItem(id: number): Observable<any>{
    return this.http.delete<any>(itemsUrl + `/${id}`);
  }

  public updateItem(updatedItem: Item): Observable<any>{
    return this.http.put<any>(itemsUrl, updatedItem);
  }

  // TRANSACTION SECTION ===========================================================================================
  public createTransaction(transaction: Transaction): Observable<any>{
    return this.http.post<any>(transactionsUrl, transaction);
  }

  public getTransactions(): Observable<Transaction[]>{
    return this.http.get<Transaction[]>(transactionsUrl);
  }

  public getTransactionById(id: number): Observable<Transaction>{
    return this.http.get<Transaction>(transactionsUrl + `/${id}`);
  }

  public deleteTransaction(id: number): Observable<any>{
    return this.http.delete<any>(transactionsUrl + `/${id}`);
  }

  public updateTransaction(updatedTransaction: Transaction): Observable<any>{
    return this.http.put<any>(transactionsUrl, updatedTransaction);
  }

  public getFirstMovementTransaction(id: number): Observable<number>{
    return this.http.get<number>(transactionsUrl + `/getMovementFirstId/${id}`);
  }
}