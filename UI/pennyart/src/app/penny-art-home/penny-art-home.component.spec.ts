import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PennyArtHomeComponent } from './penny-art-home.component';

describe('PennyArtHomeComponent', () => {
  let component: PennyArtHomeComponent;
  let fixture: ComponentFixture<PennyArtHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PennyArtHomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PennyArtHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
