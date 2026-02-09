import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductResponse, ProductAddRequest, ProductUpdateRequest } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:8080/api'; // Products API base URL

  getProducts(): Observable<ProductResponse[]> {
    return this.http.get<ProductResponse[]>(`${this.apiUrl}/products`);
  }

  getProductById(productId: string): Observable<ProductResponse> {
    return this.http.get<ProductResponse>(`${this.apiUrl}/products/search/product-id/${productId}`);
  }

  searchProducts(searchString: string): Observable<ProductResponse[]> {
    return this.http.get<ProductResponse[]>(`${this.apiUrl}/products/search/${encodeURIComponent(searchString)}`);
  }

  addProduct(product: ProductAddRequest): Observable<ProductResponse> {
    return this.http.post<ProductResponse>(`${this.apiUrl}/products`, product);
  }

  updateProduct(product: ProductUpdateRequest): Observable<ProductResponse> {
    return this.http.put<ProductResponse>(`${this.apiUrl}/products`, product);
  }

  deleteProduct(productId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/products/${productId}`);
  }
}
