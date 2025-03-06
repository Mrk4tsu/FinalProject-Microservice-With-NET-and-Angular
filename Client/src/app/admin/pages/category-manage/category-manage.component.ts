import { Component, OnInit } from '@angular/core';
import {Category} from '../../models/category.model';
import {CategoryService} from '../../services/category.service';
import {CategoryFormComponent} from './category-form/category-form.component';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-category-manage',
  imports: [
    CategoryFormComponent,
    CommonModule
  ],
  templateUrl: './category-manage.component.html',
  styleUrl: './category-manage.component.css'
})
export class CategoryManageComponent implements OnInit {
  constructor(public service: CategoryService) {
  }

  ngOnInit() {
    this.service.refreshList();
  }

  populateForm(selectedRecord: Category) {
    // this.service.formData = selectedRecord;
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDeleted(id: number) {
    if (confirm('Are you sure to delete this record?'))
      this.service.deleteCategory(id)
        .subscribe({
          next: (res: any) => {
            this.service.list = res as Category[];
          }, error: err => {
            console.log(err);
          }
        })
  }
}
