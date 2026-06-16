import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVerify } from './admin-verify';

describe('AdminVerify', () => {
  let component: AdminVerify;
  let fixture: ComponentFixture<AdminVerify>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminVerify]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVerify);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
