import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InlineEditor } from './inline-editor';

describe('InlineEditor', () => {
  let component: InlineEditor;
  let fixture: ComponentFixture<InlineEditor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InlineEditor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InlineEditor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
