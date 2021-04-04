package mcts;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;

public class TheIO {
	private static int DEFAULT_BUFFER_SIZE = 1024;

	public static String readFile(String filePath) {
		try {
			File file = new java.io.File(filePath);
			FileInputStream inputStream = new FileInputStream(file);
			String result = readStream(inputStream);
			return result;
		} catch (Exception e) {
			e.printStackTrace();
		}
		return "";
	}

	public static String readStream(InputStream in) throws IOException {
		if (in == null)
			return "";

		byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];

		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		int cnt = 0;
		while ((cnt = in.read(buffer)) > -1) {
			baos.write(buffer, 0, cnt);
		}
		baos.flush();

		in.close();
		baos.close();
		return baos.toString();
	}
	
	public static String readFile2(String filePath) {
		try {
			BufferedReader br = new BufferedReader(new FileReader(filePath));
			String r = br.readLine();
			return r;
		} catch (Exception e) {
			e.printStackTrace();
		}
		return "";
	}
	
	public static double readFile_getDouble(String filePath) {
		try {
			return Double.parseDouble(readFile(filePath));
		} catch (Exception e) {
			e.printStackTrace();
		}
		return 0;
	}
	
	public static List<Double> readFile_getDoubles(String filePath) {
		List<Double> list = new ArrayList<Double>();
		//System.out.print("FILE READ: " + filePath); 
		try {
			BufferedReader reader;
			reader = new BufferedReader(new FileReader(filePath));
			String line = reader.readLine();
			while (line != null && line != "" && !line.equals("")) {
				double d = Double.parseDouble(line);
				list.add(d);
				//System.out.print("Data: " + line); 
				// read next line
				line = reader.readLine();
			}
			reader.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return list;
	}
	

	
}
