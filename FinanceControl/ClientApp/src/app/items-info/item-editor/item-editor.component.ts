import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Group } from "../../model/group.model";
import { Repository } from "../../model/repository";
import { Location } from "@angular/common";
import { Item } from "../../model/item.model";

@Component({
  selector: 'app-item-editor',
  templateUrl: './item-editor.component.html',
  styleUrls: ['./item-editor.component.css']
})
export class ItemEditorComponent implements OnInit {
  public editorHeader: string;
  public editMode: boolean;
  public itemForm: FormGroup;
  public groups: Group[];
  public isItemNameExist: boolean; // if true - this items name already exists
  public currentItem: Item;

  constructor(
    private repository: Repository,
    private location: Location
  ) { }

  ngOnInit() {
    this.editorHeader="Create items";
    this.editMode=false;
    this.isItemNameExist=false;
    this.currentItem = new Item();

    this.itemForm=new FormGroup({
      typeControl: new FormControl(1),
      nameControl: new FormControl(''),
      groupControl: new FormControl('')
    });

    this.change_typeControl();
  }

  public onSubmit(): void{
    
  }

  public change_nameControl(): void{
    this.currentItem.name=this.nameControl.value;
    this.repository.isItemNameExists(this.currentItem).subscribe(response=>{
      this.isItemNameExist=response;
    });
  }

  public change_typeControl(): void{
    this.repository.getAllGroup(this.typeControl.value).subscribe(groups=>{
      this.groups=groups;
    });
  }

  public click_Cancel(): void{
    this.location.back();
  }

  public isInvalid(control: FormControl): boolean{
    return control.invalid && (control.dirty || control.touched);
  }

  public get typeControl(): FormControl{
    return this.itemForm.get("typeControl") as FormControl;
  }

  public get nameControl(): FormControl{
    return this.itemForm.get("nameControl") as FormControl;
  }

  public get groupControl(): FormControl{
    return this.itemForm.get("groupControl") as FormControl;
  }
}
