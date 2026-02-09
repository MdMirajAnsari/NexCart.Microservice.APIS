import { Routes } from '@angular/router';
import { UsersComponent } from './components/users/users';
import { ProductsComponent } from './components/products/products';
import { OrdersComponent } from './components/orders/orders';
import { LoginComponent } from './components/auth/login/login';
import { RegisterComponent } from './components/auth/register/register';

export const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'users', component: UsersComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
];
