import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FundosComponent } from './fundos.component';

describe('FundosComponent', () => {
  let component: FundosComponent;
  let fixture: ComponentFixture<FundosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FundosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FundosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
