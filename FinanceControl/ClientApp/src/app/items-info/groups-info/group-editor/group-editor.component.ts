import { Component, OnInit } from '@angular/core';
import { Location } from "@angular/common";
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";
import { AppStatusService } from "../../../app-status.service";
import { Group, GroupType } from "../../../model/group.model";
import { Repository } from 'src/app/model/repository';

@Component({
  selector: 'app-group-editor',
  templateUrl: './group-editor.component.html',
  styleUrls: ['./group-editor.component.css']
})
export class GroupEditorComponent implements OnInit {
  public editorHeader: string; // create or edit
  public editMode: boolean; // create or edit
  public groupForm: FormGroup;
  public currentGroup: Group;
  public isGroupNameExist: boolean; // if true - entered group name exists

  constructor(
    private location: Location,
    private appStatus: AppStatusService,
    private repository: Repository
  ) { }

  ngOnInit() {
    this.groupForm=new FormGroup({
      typeControl: new FormControl('', Validators.required),
      nameControl: new FormControl('', Validators.required),
      commentControl: new FormControl('')
    });
    
    this.editorHeader="Create group";
    this.editMode=false;

    if(this.appStatus.groupEditorCaller!=GroupType.None){
      this.typeControl.setValue(this.appStatus.groupEditorCaller);
      this.typeControl.disable();
    }

    this.currentGroup=new Group();
    this.isGroupNameExist=false;
  }

  public click_Cancel(): void{
    this.location.back();
  }

  public onSubmit(): void{
    // first check if group name is unique
    this.repository.isGroupNameExists(this.nameControl.value).subscribe(response=>{
      if(response){
        this.isGroupNameExist=true;
      } else {
        this.isGroupNameExist=false;
        this.currentGroup.type=this.typeControl.value;
        this.currentGroup.name=this.nameControl.value;
        this.currentGroup.comment=this.commentControl.value;
        this.repository.createGroup(this.currentGroup).subscribe(()=>{
          this.location.back();
        });        
      }
    });
  }

  public isInvalid(control: FormControl): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  public get typeControl(): FormControl{
    return this.groupForm.get("typeControl") as FormControl;
  }

  public get nameControl(): FormControl{
    return this.groupForm.get("nameControl") as FormControl;
  }

  public get commentControl(): FormControl{
    return this.groupForm.get("commentControl") as FormControl;
  }

  public change_nameControl():void {
    this.repository.isGroupNameExists(this.nameControl.value).subscribe(response=>{
      if(response){
        this.isGroupNameExist=true;
      } else {
        this.isGroupNameExist=false;
      }
    });
  }
}
