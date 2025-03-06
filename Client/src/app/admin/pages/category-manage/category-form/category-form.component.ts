import {Component} from '@angular/core';
import {FormsModule, NgForm} from '@angular/forms';
import {Category} from '../../../models/category.model';
import {CategoryService} from '../../../services/category.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-category-form',
  imports: [
    FormsModule
  ],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.css'
})
export class CategoryFormComponent {
  constructor(public service: CategoryService, private toastr: ToastrService) {
  }

  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if (form.valid) {
      if (this.service.formData.id == 0)
        this.insertRecord(form);
      else
        this.updateRecord(form);
    }
  }

  insertRecord(form: NgForm) {
    this.service.postCategory()
      .subscribe({
        next: (res: any) => {
          this.service.list = res as Category[];
          this.service.resetForm(form);
          this.toastr.success('Submitted successfully', 'Payment Detail');
        }, error: err => {
          console.log(err);
        }
      })
  }

  updateRecord(form: NgForm) {
    this.service.putPaymentDetail()
      .subscribe({
        next: res => {
          this.service.list = res as Category[];
          this.service.resetForm(form);
          this.toastr.info('Updated successfully', 'Payment Detail');
        }, error: err => {
          console.log(err);
        }
      })
  }
}
