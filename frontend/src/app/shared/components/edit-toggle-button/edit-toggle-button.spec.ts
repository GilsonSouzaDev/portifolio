import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditToggleButton } from './edit-toggle-button';

describe('EditToggleButton', () => {
  let component: EditToggleButton;
  let fixture: ComponentFixture<EditToggleButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditToggleButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditToggleButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
