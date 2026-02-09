export interface ProductResponse {
  productID: string;
  productName: string;
  category: CategoryOptions;
  unitPrice?: number;
  quantityInStock?: number;
}

export interface ProductAddRequest {
  productName: string;
  category: CategoryOptions;
  unitPrice?: number;
  quantityInStock?: number;
}

export interface ProductUpdateRequest {
  productID: string;
  productName?: string;
  category?: CategoryOptions;
  unitPrice?: number;
  quantityInStock?: number;
}

export enum CategoryOptions {
  Electronics = 'Electronics',
  Clothing = 'Clothing',
  Books = 'Books',
  Food = 'Food',
  Sports = 'Sports',
  Other = 'Other'
}
