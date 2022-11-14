import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PluginPlaceholderComponent } from './plugin-placeholder.component';

describe('PluginPlaceholderComponent', () => {
  let component: PluginPlaceholderComponent;
  let fixture: ComponentFixture<PluginPlaceholderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PluginPlaceholderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PluginPlaceholderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
