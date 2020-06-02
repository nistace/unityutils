namespace Utils.Id {
	public class ModificationSafeDataScriptableObject : DataScriptableObject {
		private bool                                 isModifiableInstance { get; set; }
		private ModificationSafeDataScriptableObject modifiableInstance   { get; set; }

		protected E GetModifiableInstance<E>() where E : ModificationSafeDataScriptableObject {
			if (isModifiableInstance) return (E) this;
			if (modifiableInstance) return (E) modifiableInstance;
			modifiableInstance = Instantiate(this);
			modifiableInstance.name = name;
			modifiableInstance.isModifiableInstance = true;
			return (E) modifiableInstance;
		}
	}
}