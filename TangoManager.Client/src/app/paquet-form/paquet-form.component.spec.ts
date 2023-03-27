import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaquetFormComponent } from './paquet-form.component';

describe('PaquetFormComponent', () => {
  let component: PaquetFormComponent;
  let fixture: ComponentFixture<PaquetFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaquetFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaquetFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
