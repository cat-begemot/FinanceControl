import { Injectable } from '@angular/core';
import { Account } from './account.model';
import { HttpClient } from "@angular/common/http";

const accountUrl: string = "api/accounts";

@Injectable({
  providedIn: 'root'
})
export class Repository {
  private _activeAccounts: Account[];
  get activeAccount(): Account[]{
    return this._activeAccounts;
  }

  private _inactiveAccounts: Account[];
  get inactiveAccount(): Account[]{
    return this._inactiveAccounts;
  }

  constructor(
    private http: HttpClient
  ) {
    this.getActiveAccounts();
    this.getInactiveAccounts();
  }

  private getActiveAccounts(): Account[]{
    this.http.get(accountUrl + "/active").subscribe((response: Account[])=>
      {
        this._activeAccounts=response;
      });      
      return this._activeAccounts;
  }

  private getInactiveAccounts(): Account[] {
    this.http.get(accountUrl + "/inactive").subscribe((response: Account[])=>{
      this._inactiveAccounts=response;
    });
    return this._inactiveAccounts;
  }

  public createAccount(newAccount: Account){
    this.http.post(accountUrl, newAccount).subscribe(response=>{
      this.getActiveAccounts();
    });
  }
}