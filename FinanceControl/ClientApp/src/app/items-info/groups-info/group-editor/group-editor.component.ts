import { Component, OnInit } from '@angular/core';
import { Location } from "@angular/common";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { AppStatusService } from "../../../app-status.service";
import { Group, GroupType } from "../../../model/group.model";
import { Repository } from 'src/app/model/repository';
import { ActivatedRoute } from "@angular/router";

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
    private repository: Repository,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.groupForm=new FormGroup({
      typeControl: new FormControl('', Validators.required),
      nameControl: new FormControl('', Validators.required),
      commentControl: new FormControl('')
    });
    
    this.checkEditorMode();

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

  public checkEditorMode(): void{
    let id=+this.route.snapshot.paramMap.get("id");
    if(id==0){ // Create a new group
      this.editorHeader="Create group";
      this.editMode=false;
    } else { // Edit exist group
      this.editorHeader="Edit group";
      this.editMode=true;
      this.repository.getGroupById(id).subscribe(response=>{
        this.typeControl.setValue(response.type);
        this.nameControl.setValue(response.name);
        this.commentControl.setValue(response.comment);
        this.currentGroup=response;
      });
    }
  }

  public onSubmit(): void{
    this.currentGroup.type=this.typeControl.value;
    this.currentGroup.name=this.nameControl.value;
    this.currentGroup.comment=this.commentControl.value;

    if(this.editMode){ // The item exists because it is editing now
      this.isGroupNameExist=false;
      this.repository.updateGroup(this.currentGroup).subscribe(()=>{
        this.location.back();
      });
    } else { // check weather item exists
      this.repository.isGroupNameExists(this.nameControl.value).subscribe(response=>{
        if(response){ // An error is occured - entered name exists
          this.isGroupNameExist=true;
        } else { 
          this.currentGroup.groupId=0;
          this.repository.createGroup(this.currentGroup).subscribe(()=>{
            this.location.back();
          }); 
        }
      });
    }
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
    if(this.editMode){ // The item exists because it is editing now
      this.isGroupNameExist=false;
    } else { // check weather exists
      this.repository.isGroupNameExists(this.nameControl.value).subscribe(response=>{
        if(response){
          this.isGroupNameExist=true;
        } else {
          this.isGroupNameExist=false;
        }
      });
    }
  }

  public click_deleteGroup(){
    this.repository.deleteGroup(this.currentGroup.groupId).subscribe(()=>{
      this.location.back();
    });
  }
}
