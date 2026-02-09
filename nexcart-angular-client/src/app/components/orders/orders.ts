import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { ProductsService } from '../../services/products.service';
import { OrderResponse, OrderAddRequest, OrderItemAddRequest } from '../../models/order.model';
import { ProductResponse } from '../../models/product.model';

@Component({
  selector: 'app-orders',
  imports: [CommonModule, FormsModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrdersComponent {
  private ordersService = inject(OrdersService);
  private productsService = inject(ProductsService);

  orders = signal<OrderResponse[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  // Create order form
  showCreateForm = signal<boolean>(false);
  newOrder = signal<OrderAddRequest>({
    userID: '',
    orderDate: new Date().toISOString(),
    orderItems: []
  });

  // For adding items to order
  availableProducts = signal<ProductResponse[]>([]);
  selectedProductId = signal<string>('');
  selectedQuantity = signal<number>(1);
  selectedUnitPrice = signal<number>(0);

  constructor() {
    this.loadOrders();
    this.loadProducts();
  }

  loadOrders() {
    this.loading.set(true);
    this.error.set(null);
    this.ordersService.getAllOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load orders. Please check if the API is running.');
        this.loading.set(false);
        console.error('Error loading orders:', err);
      }
    });
  }

  loadProducts() {
    this.productsService.getProducts().subscribe({
      next: (data) => {
        this.availableProducts.set(data);
      },
      error: (err) => {
        console.error('Error loading products:', err);
      }
    });
  }

  toggleCreateForm() {
    this.showCreateForm.update(val => !val);
    if (!this.showCreateForm()) {
      this.newOrder.set({
        userID: '',
        orderDate: new Date().toISOString(),
        orderItems: []
      });
    }
  }

  onProductSelect() {
    const product = this.availableProducts().find(p => p.productID === this.selectedProductId());
    if (product && product.unitPrice) {
      this.selectedUnitPrice.set(product.unitPrice);
    }
  }

  addOrderItem() {
    if (!this.selectedProductId() || this.selectedQuantity() <= 0) {
      alert('Please select a product and enter a valid quantity');
      return;
    }

    const orderItem: OrderItemAddRequest = {
      productID: this.selectedProductId(),
      quantity: this.selectedQuantity(),
      unitPrice: this.selectedUnitPrice()
    };

    this.newOrder.update(order => ({
      ...order,
      orderItems: [...order.orderItems, orderItem]
    }));

    // Reset form
    this.selectedProductId.set('');
    this.selectedQuantity.set(1);
    this.selectedUnitPrice.set(0);
  }

  removeOrderItem(index: number) {
    this.newOrder.update(order => ({
      ...order,
      orderItems: order.orderItems.filter((_, i) => i !== index)
    }));
  }

  createOrder() {
    if (!this.newOrder().userID.trim()) {
      alert('User ID is required');
      return;
    }

    if (this.newOrder().orderItems.length === 0) {
      alert('Please add at least one item to the order');
      return;
    }

    this.loading.set(true);
    this.ordersService.createOrder(this.newOrder()).subscribe({
      next: (order) => {
        this.orders.update(orders => [order, ...orders]);
        this.toggleCreateForm();
        this.loading.set(false);
        alert('Order created successfully!');
      },
      error: (err) => {
        this.error.set('Failed to create order.');
        this.loading.set(false);
        console.error('Error creating order:', err);
        alert('Failed to create order. Please check the console for details.');
      }
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  calculateTotal(order: OrderResponse): number {
    return order.orderItems.reduce((sum, item) => sum + item.totalPrice, 0);
  }

  updateOrderUserId(value: string) {
    this.newOrder.update(o => ({ ...o, userID: value }));
  }

  updateOrderDate(value: string) {
    this.newOrder.update(o => ({ ...o, orderDate: value }));
  }}
