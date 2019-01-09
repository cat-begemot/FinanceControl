import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { formGroupNameProvider } from '@angular/forms/src/directives/reactive_directives/form_group_name';
import { formControlBinding } from '@angular/forms/src/directives/reactive_directives/form_control_directive';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  public signUpForm: FormGroup;

  constructor(

  ) { }

  ngOnInit() {
    this.signUpForm=new FormGroup({
      loginControl: new FormControl('', Validators.required),
      passwordControl: new FormControl('', Validators.required),
      passwordConfirmationControl: new FormControl('', Validators.required)
    });
  }

  public isInvalid(control: AbstractControl): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  public get loginControl(): AbstractControl{
    return this.signUpForm.get("logonControl");
  }

  public get passwordControl(): AbstractControl{
    return this.signUpForm.get("passwordControl");
  }

  public get passwordConfiramtionControl(): AbstractControl{
    return this.signUpForm.get("passwordConfirmationControl");
  }
}
