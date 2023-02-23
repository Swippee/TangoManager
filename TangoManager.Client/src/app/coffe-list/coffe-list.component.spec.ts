import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoffeListComponent } from './coffe-list.component';

describe('CoffeListComponent', () => {
  let component: CoffeListComponent;
  let fixture: ComponentFixture<CoffeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoffeListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoffeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
