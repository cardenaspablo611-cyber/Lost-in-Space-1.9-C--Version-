import os

def rename_files_in_directory(directory_path):
    for filename in os.listdir(directory_path):
        if "Enemie" in filename:
            # Replace 'Enemie' with 'Enemy' in the filename
            new_filename = filename.replace("Enemie", "Enemy")
            
            # Get the full paths to the old and new filenames
            old_file = os.path.join(directory_path, filename)
            new_file = os.path.join(directory_path, new_filename)
            
            # Check if the new file name already exists
            if os.path.exists(new_file):
                print(f"Skipping {old_file} -> {new_file} (file already exists)")
            else:
                # Rename the file
                os.rename(old_file, new_file)
                print(f"Renamed: {old_file} -> {new_file}")

# Use a raw string for the directory path
directory_path = r"C:\Users\carde\Unity Proyects\Lost In Space\Assets\Prefabs\Enemies"
rename_files_in_directory(directory_path)

