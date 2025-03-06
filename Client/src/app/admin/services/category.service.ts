import {Injectable} from '@angular/core';
import {NgForm} from '@angular/forms';
import {Category} from '../models/category.model';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  url: string = environment.baseUrl + 'category';
  list: Category[] = [];
  formData: Category = new Category();
  formSubmitted: boolean = false;

  constructor(private http: HttpClient) {
  }

  refreshList() {
    this.http.get(this.url + '/list').subscribe({
      next: (res: any) => {
        this.list = res.data as Category[];
      },
      error: err => {
        console.log(err);
      }
    })
  }

  postCategory() {
    return this.http.post(this.url, this.formData);
  }

  deleteCategory(id: number) {
    return this.http.delete(this.url + '/delete?categoryId=' + id);
  }

  putPaymentDetail() {
    return this.http.put(this.url + '/update?categoryId=' + this.formData.id, this.formData);
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.formData = new Category();
    this.formSubmitted = false;
  }
}
