import os

def count_lines_of_code(directory):
    total_lines = 0
    file_count = 0
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith(".cs"):  # Only count lines in C# files
                file_count += 1
                file_path = os.path.join(root, file)
                with open(file_path, 'r', encoding='utf-8') as f:
                    lines = f.readlines()
                    lines_count = len(lines)
                    total_lines += lines_count
                    # Print the relative path from the root directory
                    relative_path = os.path.relpath(file_path, directory)
                    print(f"[{file_count}] {relative_path}: {lines_count} lines")
    
    print(f"\nTotal files: {file_count}")
    print(f"Total lines of code in C# files: {total_lines}")

# Replace 'your_directory_path' with the path to the root directory of your project
directory_path = r"C:\Users\carde\Unity Proyects\Lost In Space\Assets\Scripts"
count_lines_of_code(directory_path)
