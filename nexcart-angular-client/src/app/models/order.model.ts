export interface OrderResponse {
  orderID: string;
  userID: string;
  totalBill: number;
  orderDate: string;
  orderItems: OrderItemResponse[];
  userPersonName?: string;
  email?: string;
}

export interface OrderItemResponse {
  orderItemID: string;
  orderID: string;
  productID: string;
  productName?: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface OrderAddRequest {
  userID: string;
  orderDate: string;
  orderItems: OrderItemAddRequest[];
}

export interface OrderItemAddRequest {
  productID: string;
  quantity: number;
  unitPrice: number;
}
