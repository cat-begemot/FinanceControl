import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { Location } from "@angular/common";
import { Router } from "@angular/router";
import { Repository } from "../../model/repository";
import { AuthenticationService } from "../authentication.service";
import { Target } from "../../model/helper.model";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  public signUpForm: FormGroup;
  public errorLogin: boolean;
  public errorPasswordConfirmation: boolean;
  public targetComponent: Target;

  constructor(
    private location: Location,
    private repository: Repository,
    public authService: AuthenticationService,
    private routerNav: Router
  ) { }

  ngOnInit() {
    this.targetComponent=Target.Signup;
    this.authService.isSeedData=true;
    this.signUpForm=new FormGroup({
      loginControl: new FormControl('', Validators.required),
      passwordControl: new FormControl('', Validators.required),
      passwordConfirmationControl: new FormControl('', Validators.required),
      isSeedDataControl: new FormControl(this.authService.isSeedData)
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

  public get isSeedDataControl(): FormControl{
    return this.signUpForm.get("isSeedDataControl") as FormControl;
  }

  public click_Cancel():void{
    this.location.back();
  }

  public change_isSeedDataControl(): void{
    this.authService.isSeedData=!this.authService.isSeedData;
  }

  // if return true - there was succesfully registration. Else - error (the name is already existed)
  public onSubmit(): void {
    this.authService.name=this.loginControl.value;
    this.authService.password=this.passwordControl.value;
    
    this.authService.createUserProfile().subscribe(response=>{
      if(response){
        this.authService.isSuccessfullyCreated=true;
        this.authService.login().subscribe(response=>{
          if(response){ // if login is successfull
            if(this.authService.isSeedData){
              this.repository.seedData().subscribe(()=>{
                this.routerNav.navigate(["/accounts"]);
              });
            } else {
              this.routerNav.navigate(["/accounts"]);
            }
          } else{ // login is failed
              
          }
        });
      }
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
