# Circular Loading Component with BehaviorSubject

## Overview
A global circular loading indicator that uses RxJS `BehaviorSubject` to manage loading state across the entire application. The component shows a beautiful animated circular spinner overlay when operations are in progress.

## Features
- ✅ Global loading state management using BehaviorSubject
- ✅ Animated circular spinner with 4 rings
- ✅ Customizable loading messages
- ✅ Backdrop blur effect
- ✅ Responsive design (mobile, tablet, desktop)
- ✅ Accessible and performant (OnPush change detection)

## Files Created
- `src/app/services/loading.service.ts` - BehaviorSubject-based loading service
- `src/app/components/circular-loading/circular-loading.ts` - Component
- `src/app/components/circular-loading/circular-loading.html` - Template
- `src/app/components/circular-loading/circular-loading.css` - Styles

## Usage

### 1. Basic Setup (Already Done)
The circular loading component is already integrated in the root app:
```html
<!-- app.html -->
<app-circular-loading></app-circular-loading>
```

### 2. Using in Components

**Import the LoadingService:**
```typescript
import { Component, inject } from '@angular/core';
import { LoadingService } from '../../services/loading.service';

@Component({...})
export class MyComponent {
  private loadingService = inject(LoadingService);

  loadData() {
    // Show loading with custom message
    this.loadingService.show('Loading data...');
    
    // Your API call
    this.api.getData().subscribe({
      next: (data) => {
        this.loadingService.hide();
      },
      error: () => {
        this.loadingService.hide();
      }
    });
  }
}
```

### 3. LoadingService Methods

**`show(message?: string): void`**
- Displays the circular loading overlay
- Optional message parameter (default: 'Loading...')
```typescript
this.loadingService.show('Processing your request...');
this.loadingService.show(); // Uses default message
```

**`hide(): void`**
- Hides the circular loading overlay
```typescript
this.loadingService.hide();
```

**`isLoading(): boolean`**
- Returns current loading state
```typescript
if (this.loadingService.isLoading()) {
  console.log('Currently loading');
}
```

**`getMessage(): string`**
- Returns current loading message
```typescript
const msg = this.loadingService.getMessage();
```

### 4. Observable Properties

**`loading$: Observable<boolean>`**
- Subscribe to loading state changes
```typescript
this.loadingService.loading$.subscribe(isLoading => {
  console.log('Loading:', isLoading);
});
```

**`loadingMessage$: Observable<string>`**
- Subscribe to loading message changes
```typescript
this.loadingService.loadingMessage$.subscribe(message => {
  console.log('Message:', message);
});
```

## Example Implementation

```typescript
@Component({...})
export class ProductsComponent {
  private api = inject(ApiService);
  private loadingService = inject(LoadingService);

  loadProducts() {
    this.loadingService.show('Fetching products...');
    
    this.api.getProducts().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loadingService.hide();
      },
      error: (error) => {
        console.error('Error:', error);
        this.loadingService.hide();
      }
    });
  }

  addProduct(product: Product) {
    this.loadingService.show('Adding product...');
    
    this.api.addProduct(product).subscribe({
      next: (newProduct) => {
        this.products.update(p => [...p, newProduct]);
        this.loadingService.hide();
      },
      error: (error) => {
        console.error('Error:', error);
        this.loadingService.hide();
      }
    });
  }
}
```

## Styling

The component uses Tailwind CSS color values. To customize colors, edit `circular-loading.css`:

```css
/* Change primary color from blue (#3b82f6) to another color */
.spinner-ring {
  border: 4px solid #YOUR_COLOR;
  border-color: #YOUR_COLOR transparent transparent transparent;
}
```

## Performance
- Uses `ChangeDetectionStrategy.OnPush` for optimal performance
- Async pipe for reactive updates
- BehaviorSubject for efficient state management
- Minimal re-renders

## Browser Support
- Chrome/Edge (latest)
- Firefox (latest)
- Safari (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

## Accessibility
- Backdrop blur provides visual feedback
- High contrast spinner for visibility
- Responsive text sizing
- Semantic HTML structure
