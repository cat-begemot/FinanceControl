import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from "../authentication.service";
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { Repository } from 'src/app/model/repository';
import { Router } from "@angular/router";
import { AppStatusService } from 'src/app/app-status.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {
  public loginForm: FormGroup;

  constructor(
    private authService: AuthenticationService,
    private repository: Repository,
    private routerNav: Router,
    private appStatus: AppStatusService
  ) { }

  ngOnInit() {
    this.loginForm=new FormGroup({
      loginControl: new FormControl('', Validators.required),
      passwordControl: new FormControl('', Validators.required)
    });
    this.loginForm.valueChanges.subscribe(()=>this.appStatus.validCredentials=true);
  }

  public onSubmit(){
    this.authService.name=this.loginForm.value["loginControl"];
    this.authService.password=this.loginForm.value["passwordControl"];
    
    if (!this.authService.authenticated){
      this.authService.login().subscribe(response=>{
        if(response){
          // successfully authorization
          this.routerNav.navigate(["/accounts"]);
        } else{
          // invalid credentials
          this.loginForm.setValue({loginControl: "", passwordControl: ""});
          this.loginForm.reset();
          this.appStatus.validCredentials=false;
        }
      });       
    }
  }

  // check whether is control value is invalid and it is pristin and untouched
  public isInvalid(control: AbstractControl): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  // return control from FormGroup
  public getLoginControl(){
    return this.loginForm.get("loginControl");
  }

  public getPasswordControl(){
    return this.loginForm.get("passwordControl");
  }
}
