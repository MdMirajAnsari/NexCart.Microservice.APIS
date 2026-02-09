import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { ProductResponse, ProductAddRequest, CategoryOptions } from '../../models/product.model';

@Component({
  selector: 'app-products',
  imports: [CommonModule, FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductsComponent {
  private productsService = inject(ProductsService);

  products = signal<ProductResponse[]>([]);
  filteredProducts = signal<ProductResponse[]>([]);
  searchTerm = signal<string>('');
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  // Add product form
  showAddForm = signal<boolean>(false);
  newProduct = signal<ProductAddRequest>({
    productName: '',
    category: CategoryOptions.Other,
    unitPrice: 0,
    quantityInStock: 0
  });

  categories = Object.values(CategoryOptions);

  constructor() {
    this.loadProducts();
  }

  loadProducts() {
    this.loading.set(true);
    this.error.set(null);
    this.productsService.getProducts().subscribe({
      next: (data) => {
        this.products.set(data);
        this.filteredProducts.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load products. Please check if the API is running.');
        this.loading.set(false);
        console.error('Error loading products:', err);
      }
    });
  }

  searchProducts() {
    if (!this.searchTerm().trim()) {
      this.filteredProducts.set(this.products());
      return;
    }

    this.loading.set(true);
    this.productsService.searchProducts(this.searchTerm()).subscribe({
      next: (data) => {
        this.filteredProducts.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to search products.');
        this.loading.set(false);
        console.error('Error searching products:', err);
      }
    });
  }

  toggleAddForm() {
    this.showAddForm.update(val => !val);
    if (!this.showAddForm()) {
      this.newProduct.set({
        productName: '',
        category: CategoryOptions.Other,
        unitPrice: 0,
        quantityInStock: 0
      });
    }
  }

  addProduct() {
    if (!this.newProduct().productName.trim()) {
      alert('Product name is required');
      return;
    }

    this.loading.set(true);
    this.productsService.addProduct(this.newProduct()).subscribe({
      next: (product) => {
        this.products.update(prods => [...prods, product]);
        this.filteredProducts.set(this.products());
        this.toggleAddForm();
        this.loading.set(false);
        alert('Product added successfully!');
      },
      error: (err) => {
        this.error.set('Failed to add product.');
        this.loading.set(false);
        console.error('Error adding product:', err);
        alert('Failed to add product. Please check the console for details.');
      }
    });
  }

  deleteProduct(productId: string) {
    if (!confirm('Are you sure you want to delete this product?')) {
      return;
    }

    this.loading.set(true);
    this.productsService.deleteProduct(productId).subscribe({
      next: () => {
        this.products.update(prods => prods.filter(p => p.productID !== productId));
        this.filteredProducts.set(this.products());
        this.loading.set(false);
        alert('Product deleted successfully!');
      },
      error: (err) => {
        this.error.set('Failed to delete product.');
        this.loading.set(false);
        console.error('Error deleting product:', err);
        alert('Failed to delete product. Please check the console for details.');
      }
    });
  }

  updateProductName(value: string) {
    this.newProduct.update(p => ({ ...p, productName: value }));
  }

  updateProductCategory(value: CategoryOptions) {
    this.newProduct.update(p => ({ ...p, category: value }));
  }

  updateProductPrice(value: number) {
    this.newProduct.update(p => ({ ...p, unitPrice: value }));
  }

  updateProductQuantity(value: number) {
    this.newProduct.update(p => ({ ...p, quantityInStock: value }));
  }
}
