import os

def search_debug_log_in_cs_files(directory):
    # Loop through all files in the given directory
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith(".cs"):
                file_path = os.path.join(root, file)
                # Open and read the file
                with open(file_path, 'r') as cs_file:
                    lines = cs_file.readlines()
                    # Loop through each line to find "Debug.Log("
                    for line_number, line in enumerate(lines, start=1):
                        if "Debug.Log(" in line:
                            print(f"Found in {file_path} at line {line_number}: {line.strip()}")

# Replace 'your_directory_path' with the path to the directory you want to search
directory_to_search = 'C:\\Users\\carde\\Unity Proyects\\Lost In Space\\Assets\\Scripts\\General'
search_debug_log_in_cs_files(directory_to_search)