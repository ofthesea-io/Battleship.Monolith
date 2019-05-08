import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GamingGridComponent } from './gaming-grid.component';

describe('GamingGridComponent', () => {
  let component: GamingGridComponent;
  let fixture: ComponentFixture<GamingGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GamingGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GamingGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
