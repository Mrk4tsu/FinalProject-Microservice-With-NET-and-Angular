import {inject, Injectable, makeStateKey, TransferState} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { environment } from '../../../environments/environment';
import {BehaviorSubject, catchError, map, of, tap} from 'rxjs';
import {Category} from '../models/category.class';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);
  private transferState = inject(TransferState);
  private baseCategoryUrl = environment.baseUrl + 'category/';

  private categoriesSubject = new BehaviorSubject<Category[]>([]);
  categories$ = this.categoriesSubject.asObservable();

  loadCategories(): void {
    const CATEGORIES_KEY = makeStateKey<Category[]>('categories');

    if (this.transferState.hasKey(CATEGORIES_KEY)) {
      const categories = this.transferState.get(CATEGORIES_KEY, []);
      this.categoriesSubject.next(categories);
      this.transferState.remove(CATEGORIES_KEY);
    } else {
      this.http.get(this.baseCategoryUrl + 'list').pipe(
        map((res: any) => res.data as Category[]),
        tap(categories => {
            this.transferState.set(CATEGORIES_KEY, categories);
        }),
        catchError(error => {
          console.log(error);
          return of([]);
        })
      ).subscribe(categories => {
        this.categoriesSubject.next(categories);
      });
    }
  }
}
