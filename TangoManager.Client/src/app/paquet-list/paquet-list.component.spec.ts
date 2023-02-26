import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaquetListComponent } from './paquet-list.component';

describe('PaquetListComponent', () => {
  let component: PaquetListComponent;
  let fixture: ComponentFixture<PaquetListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaquetListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaquetListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
