import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderResponse, OrderAddRequest } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:8080/api'; // Orders API base URL

  getAllOrders(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/orders`);
  }

  getOrderById(orderId: string): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.apiUrl}/orders/${orderId}`);
  }

  createOrder(order: OrderAddRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/orders`, order);
  }
}
