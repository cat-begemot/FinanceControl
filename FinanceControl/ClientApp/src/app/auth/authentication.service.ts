import { Injectable } from '@angular/core';
import { Repository } from "../model/repository";
import { Router } from "@angular/router";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  // TODO: create ViewModel credentials class
  public name: string;
  public password: string;
  public isSeedData: boolean;

  public callBackUrl: string;
  public authenticated: boolean;
  public isSuccessfullyCreated: boolean;

  constructor(
    private repository: Repository,
    private routerNav: Router
  ) { 
    // Check whether the user is already had authenticated
    this.repository.isAuthenticated().subscribe(response=>{
      if(response){
        this.authenticated=true;
      } else {
        this.authenticated=false;
        this.routerNav.navigate(["/login"]);
      }
    });
    
    this.isSuccessfullyCreated=false;
    this.isSeedData=true; // by default application seeds the database with sample data
  }

  public login(): Observable<boolean>{
    let obs:  Observable<any> = this.repository.login(this.name, this.password);

    obs.subscribe(response=>{
      if(response==true)
      {
        this.authenticated=true;
        this.password=null; 
      }
    });
  
    return obs;
  }

  public logout(): void{
    this.authenticated=false;
    this.repository.logout().subscribe(()=>{});
  }

  public createUserProfile(): Observable<boolean>{
    this.isSuccessfullyCreated=false;
    
    let obs: Observable<any> = this.repository.createUserProfile(this.name, this.password);

    /*
    obs.subscribe(response=>{
      this.isSuccessfullyCreated=true;
    });
*/

    return obs;
  }
}
