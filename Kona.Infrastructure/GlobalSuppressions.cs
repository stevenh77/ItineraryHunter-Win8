// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#SetProperty`1(!!0&,!!0,System.String)", Justification = "VS Template Code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#SetProperty`1(!!0&,!!0,System.String)", Justification = "VS Template Code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#OnPropertyChanged(System.String)", Justification = "VS Template Code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope = "member", Target = "Kona.Infrastructure.ViewModelLocator.#AutoWireViewModelProperty")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "Kona.Infrastructure.IFlyoutViewModel")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "member", Target = "Kona.Infrastructure.IFlyoutViewModel.#CloseFlyout")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "Kona.Infrastructure.ISettingsCharmService.#ShowFlyout(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "member", Target = "Kona.Infrastructure.ISettingsCharmService.#ShowFlyout(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyouts", Scope = "namespace", Target = "Kona.Infrastructure.Flyouts")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "Kona.Infrastructure.Flyouts.StandardFlyoutSize")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "Kona.Infrastructure.Flyouts.FlyoutView")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "member", Target = "Kona.Infrastructure.Flyouts.FlyoutView.#FlyoutSize")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "Kona.Infrastructure.Flyouts.FlyoutView.#.ctor(System.String,System.String,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "Kona.Infrastructure.Flyouts.IFlyoutViewModel")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "member", Target = "Kona.Infrastructure.Flyouts.IFlyoutViewModel.#CloseFlyout")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#SetProperty`1(!!0&,!!0,System.Boolean,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#SetProperty`1(!!0&,!!0,System.Boolean,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.BindableBase.#RaiseErrorsChanged(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Kona.Infrastructure.Flyouts")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#EmptyErrorsCollection", Justification = "A ReadOnlyCollection is inmutable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#Errors", Justification = "A ReadOnlyCollection is inmutable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#GetAllErrors()", Justification = "It's not appropiate to use a property because the method always returns a new instance of a ReadOnlyCollection and the class implements INotifyPropertyChanged.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "e", Scope = "member", Target = "Kona.Infrastructure.VisualStateAwarePage.#StopLayoutUpdates(System.Object,Windows.UI.Xaml.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.IRestorableStateService.#RaiseAppRestored()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "e", Scope = "member", Target = "Kona.Infrastructure.VisualStateAwarePage.#StartLayoutUpdates(System.Object,Windows.UI.Xaml.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Kona.Infrastructure.SuspensionManager.#RegisterFrame(Windows.UI.Xaml.Controls.Frame,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Kona.Infrastructure.SuspensionManager.#SessionStateForFrame(Windows.UI.Xaml.Controls.Frame)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Kona.Infrastructure.SuspensionManager.#SessionStateForFrame(Windows.UI.Xaml.Controls.Frame)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Scope = "type", Target = "Kona.Infrastructure.SuspensionManagerException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SuspensionManager", Scope = "member", Target = "Kona.Infrastructure.SuspensionManagerException.#.ctor(System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "e", Scope = "member", Target = "Kona.Infrastructure.SuspensionManagerException.#.ctor(System.Exception)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Kona.Infrastructure.VisualStateAwarePage.#OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#SetAllErrors(System.Collections.Generic.IDictionary`2<System.String,System.Collections.ObjectModel.ReadOnlyCollection`1<System.String>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.BindableValidator.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ValidatorType", Scope = "member", Target = "Kona.Infrastructure.AsyncValidationAttribute.#IsValid(System.Object,System.ComponentModel.DataAnnotations.ValidationContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AsyncValidationAttribute", Scope = "member", Target = "Kona.Infrastructure.AsyncValidationAttribute.#IsValid(System.Object,System.ComponentModel.DataAnnotations.ValidationContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ValidationResult", Scope = "member", Target = "Kona.Infrastructure.AsyncValidationAttribute.#IsValid(System.Object,System.ComponentModel.DataAnnotations.ValidationContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "Kona.Infrastructure.ISettingsCharmService.#ShowFlyout(System.String,System.Object,System.Action)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "member", Target = "Kona.Infrastructure.ISettingsCharmService.#ShowFlyout(System.String,System.Object,System.Action)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "Kona.Infrastructure.ValidatableBindableBase.#SetProperty`1(!!0&,!!0,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.IValidatableBindableBase.#SetAllErrors(System.Collections.Generic.IDictionary`2<System.String,System.Collections.ObjectModel.ReadOnlyCollection`1<System.String>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validatable", Scope = "type", Target = "Kona.Infrastructure.ValidatableBindableBase")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.IValidatableBindableBase.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.IValidatableBindableBase.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validatable", Scope = "type", Target = "Kona.Infrastructure.IValidatableBindableBase")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Scope = "member", Target = "Kona.Infrastructure.DependencyPropertyChangedHelper.#HelperProperty")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Kona.Infrastructure.DependencyPropertyChangedHelper.#PropertyChanged")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Kona.Infrastructure.Interfaces")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validatable", Scope = "type", Target = "Kona.Infrastructure.Interfaces.IValidatableBindableBase")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Kona.Infrastructure.Interfaces.IValidatableBindableBase.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.Interfaces.IValidatableBindableBase.#GetAllErrors()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Kona.Infrastructure.Interfaces.IValidatableBindableBase.#SetAllErrors(System.Collections.Generic.IDictionary`2<System.String,System.Collections.ObjectModel.ReadOnlyCollection`1<System.String>>)")]
