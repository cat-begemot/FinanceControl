import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { Location } from "@angular/common";
import { Router } from "@angular/router";
import { Repository } from "../../model/repository";
import { AuthenticationService } from "../authentication.service";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  public signUpForm: FormGroup;
  public errorLogin: boolean;
  public errorPasswordConfirmation: boolean;

  constructor(
    private location: Location,
    private repository: Repository,
    private authService: AuthenticationService,
    private routerNav: Router
  ) { }

  ngOnInit() {
    this.signUpForm=new FormGroup({
      loginControl: new FormControl('', Validators.required),
      passwordControl: new FormControl('', Validators.required),
      passwordConfirmationControl: new FormControl('', Validators.required)
    });
    this.errorLogin=false;
    this.errorPasswordConfirmation=false;
  }

  public isInvalid(control: AbstractControl): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  // valid and changed or touched
  public isValid(control: AbstractControl): boolean{
    return control.valid && (control.dirty || control.touched);
  }

  public get loginControl(): AbstractControl{
    return this.signUpForm.get("loginControl");
  }

  public get passwordControl(): AbstractControl{
    return this.signUpForm.get("passwordControl");
  }

  public get passwordConfiramtionControl(): AbstractControl{
    return this.signUpForm.get("passwordConfirmationControl");
  }

  public click_Cancel():void{
    this.location.back();
  }

  // if return true - there was succesfully registration. Else - error (the name is already existed)
  public onSubmit(): void {
    this.authService.name=this.loginControl.value;
    this.authService.password=this.passwordControl.value;
    this.authService.createUserProfile().subscribe(response=>{
        this.authService.login().subscribe(response=>{
          if(response){
            this.routerNav.navigate(["/accounts"]);
          } else{
            // login is failed
          }
        });
    });
  }

  // check wheather the login hasn't already existed
  public change_checkLogin(): void{
    this.repository.isLoginExist(this.loginControl.value).subscribe(response=>{
      if(response==true){
        this.errorLogin=true;
      } else{
        this.errorLogin=false;
      }
    });
  }

  public change_pswConfirm(): void{
    if(this.passwordControl.value!=this.passwordConfiramtionControl.value){
      this.errorPasswordConfirmation=true;
    } else{
      this.errorPasswordConfirmation=false;
    }
  }
}
