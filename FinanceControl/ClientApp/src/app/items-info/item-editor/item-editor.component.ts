import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Group, GroupType } from "../../model/group.model";
import { Repository } from "../../model/repository";
import { Location } from "@angular/common";
import { Item } from "../../model/item.model";
import { ActivatedRoute } from "@angular/router";

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
    private location: Location,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.checkEditMode();
    this.isItemNameExist=false;
    this.currentItem = new Item();

    this.itemForm=new FormGroup({
      typeControl: new FormControl(1, Validators.required),
      nameControl: new FormControl('', Validators.required),
      groupControl: new FormControl('', Validators.required)
    });
  }

  private checkEditMode(): void{
    let id: number = +this.activatedRoute.snapshot.paramMap.get("id");
    if(id==0){ // create mode
      this.editorHeader="Create item";
      this.editMode=false;
    } else { // edit mode
      this.editorHeader="Edit item";
      this.editMode=true;
      this.repository.getItemById(id).subscribe(item=>{
        this.currentItem=item;
        if(this.currentItem.group.type==GroupType.Expense){ // if item is in Expense group
          this.typeControl.setValue(GroupType.Expense);
        } else { // if item is in Income group
          this.typeControl.setValue(GroupType.Income);
        }
        this.change_typeControl();
        this.nameControl.setValue(this.currentItem.name);
        this.groupControl.setValue(this.currentItem.groupId);
      });
    }
  }

  public change_typeControl(): void{
    this.repository.getAllGroup(this.typeControl.value).subscribe(groups=>{
      this.groups=groups;
    });
    this.groupControl.setValue(null);
    this.groupControl.markAsPristine();
    this.groupControl.markAsUntouched();
  }

  public onSubmit(): void{
    this.currentItem.groupId=this.groupControl.value;
    this.currentItem.name=this.nameControl.value;
    if(this.editMode){ // edit exists item
      this.currentItem.group=null;
      this.repository.updateItem(this.currentItem).subscribe(()=>this.location.back());
    } else { // create new item
      this.repository.createItem(this.currentItem).subscribe(()=>this.location.back());
    }
  }

  public click_deleteItem(): void{
    this.repository.deleteItem(this.currentItem.itemId).subscribe(()=>this.location.back());
  }

  public change_nameControl(): void{
    this.currentItem.name=this.nameControl.value;
    this.repository.isItemNameExists(this.currentItem).subscribe(response=>{
      this.isItemNameExist=response;
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
